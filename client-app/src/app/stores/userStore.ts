import { makeAutoObservable, runInAction } from "mobx";
import { User, UserFormValues } from "../models/user";
import agent from "../api/agent";
import { store } from "./store";
import { router } from "../router/Routes";

export default class UserStore {
  user: User | null = null;
  refreshTokenTimeout: any

  constructor() {
    makeAutoObservable(this);
  }

  get isLoggedIn() {
    return !!this.user;
  }

  login = async (creds: UserFormValues) => {
    try {
      const user = await agent.Account.login(creds);
      store.commonStore.setToken(user.token);
      this.startRefreshTokenTime(user)
      runInAction(() => (this.user = user));
      router.navigate("/activities");
      store.modalStore.closeModal();
    } catch (error) {
      throw error;
    }
  };

  logout = () => {
    store.commonStore.setToken(null);
    window.localStorage.removeItem("jwt");
    this.user = null;
    router.navigate("/");
  };

  getUser = async () => {
    try {
      const user = await agent.Account.current();
      store.commonStore.setToken(user.token);
      runInAction(() => (this.user = user));
      this.startRefreshTokenTime(user)
    } catch (error) {
      console.log(error);
    }
  };

  register = async (creds: UserFormValues) => {
    try {
      const user = await agent.Account.register(creds);
      store.commonStore.setToken(user.token);
      this.startRefreshTokenTime(user)
      runInAction(() => (this.user = user));
      router.navigate("/activities");
      store.modalStore.closeModal();
    } catch (error) {
      throw error;
    }
  };

  setImage = (image: string) => {
    if(this.user) this.user.image = image
  }

  refreshToken = async () => {
    this.stopRefreshTokenTime()
    try {
      const user = await agent.Account.refreshToken();
      runInAction(() => this.user = user);
      store.commonStore.setToken(user.token);
      this.startRefreshTokenTime(user)
    } catch (error) {
      console.log(error);
    }
  }

  private startRefreshTokenTime(user: User) {
    const jwtToken = JSON.parse(atob(user.token.split('.')[1]));
    const expires = new Date(jwtToken.exp * 1000);
    const timeout = expires.getTime() - Date.now() - (60 * 1000);
    this.refreshTokenTimeout = setTimeout(this.refreshToken, timeout);
  }

  private stopRefreshTokenTime(){
    clearTimeout(this.refreshTokenTimeout)
  }
}
