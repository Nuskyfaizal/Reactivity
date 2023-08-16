import { observer } from "mobx-react-lite";
import * as React from "react";
import { useEffect } from "react";
import { Grid } from "semantic-ui-react";
import LoadingComponent from "../../../layout/LoadingComponents";
import { useStore } from "../../../stores/store";
import ActivityList from "./ActivityList";
import ActivityFilters from './ActivityFilters';

function ActivityDashboard() {
  const { activityStore } = useStore();
  const {loadActivities, activityRegistry} = activityStore

  useEffect(() => {
    if(activityRegistry.size <= 1) loadActivities();
  }, [activityRegistry.size, loadActivities]);

  if (activityStore.loadingInitial)
    return <LoadingComponent content="Loading app" />;

  return (
    <Grid>
      <Grid.Column width="10">
        <ActivityList />
      </Grid.Column>
      <Grid.Column width="6">
       <ActivityFilters/>
      </Grid.Column>
    </Grid>
  );
}

export default observer(ActivityDashboard);
