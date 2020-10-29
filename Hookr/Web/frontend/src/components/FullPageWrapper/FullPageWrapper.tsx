import React from "react";
import "./FullPageWrapper.css";
import makeStyles from "@material-ui/core/styles/makeStyles";
import { createStyles, Theme } from "@material-ui/core";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    fullPage: {
      backgroundColor: theme.palette.background.default,
    },
  })
);

const FullPageWrapper: React.FC<React.PropsWithChildren<
  React.HTMLAttributes<HTMLElement>
>> = (props: React.PropsWithChildren<React.HTMLAttributes<HTMLElement>>) => {
  const styles = useStyles();
  return (
    <div
      className={`${props.className} ${styles.fullPage} FullPageWrapper`}
      data-testid="FullPageWrapper"
    >
      {props.children}
    </div>
  );
};

export default FullPageWrapper;
