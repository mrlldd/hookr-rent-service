import React from "react";
import "./LoadingSpinner.css";
import { CircularProgress, createStyles } from "@material-ui/core";
import { makeStyles } from "@material-ui/core/styles";
import { Theme } from "@material-ui/core/styles/createMuiTheme";

interface Props {
  size: number;
  loading: boolean;
}

function useStyles(size: number) {
  return makeStyles((theme: Theme) =>
    createStyles({
      dynamicContainer: {
        margin: theme.spacing(1),
        marginTop: -size / 2,
        marginLeft: -size / 2,
      },
    })
  )();
}

const LoadingSpinner: React.FC<React.PropsWithChildren<Props>> = (
  props: React.PropsWithChildren<Props>
) => {
  const styleClasses = useStyles(props.size);
  return (
    <div className="LoadingSpinner" data-testid="LoadingSpinner">
      {props.loading && (
        <div className={"spinner-container"}>
          <CircularProgress
            className={`${styleClasses.dynamicContainer} spinner`}
            size={props.size}
          />
        </div>
      )}
      {props.children}
    </div>
  );
};

export default LoadingSpinner;
