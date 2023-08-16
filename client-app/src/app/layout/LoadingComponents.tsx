import React from "react";
import { Loader, Dimmer } from "semantic-ui-react";

interface Props {
    inverted?: boolean;
    content: string
}

function LoadingComponent(
  { inverted = true, content = "Loading..." } :Props
)

{
  return (
    <Dimmer active={true} inverted={inverted}>
      <Loader content={content} />
    </Dimmer>
  );
}

export default LoadingComponent;