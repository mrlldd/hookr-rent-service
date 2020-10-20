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
      spinnerContainer: {
        margin: theme.spacing(1),
        position: "absolute",
        top: "50%",
        left: "50%",
        marginTop: -size / 2,
        marginLeft: -size / 2,
        zIndex: 1,
      },
    })
  )();
}

const LoadingSpinner: React.FC<Props> = (
  props: React.PropsWithChildren<Props>
) => {
  const styleClasses = useStyles(props.size);
  return (
    <div className="LoadingSpinner" data-testid="LoadingSpinner">
      {props.loading && (
        <CircularProgress
          className={styleClasses.spinnerContainer}
          size={props.size}
        />
      )}
      {props.children}
    </div>
  );
};

export default LoadingSpinner;
