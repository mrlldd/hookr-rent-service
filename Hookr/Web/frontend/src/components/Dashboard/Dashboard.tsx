import React, { createRef, useContext, useState } from "react";
import "./Dashboard.css";
import FullPageWrapper from "../FullPageWrapper/FullPageWrapper";

import makeStyles from "@material-ui/core/styles/makeStyles";
import { createStyles, SwipeableDrawer, Theme } from "@material-ui/core";
import { getFromLocalStorage } from "../../context/local-storage-utils";
import { UserContextInstance } from "../../context/user/user-context-instance";
import Avatar from "@material-ui/core/Avatar";

const Dashboard: React.FC = () => {
  return (
    <FullPageWrapper className="Dashboard" data-testid="Dashboard">
      <RealDashboard />
    </FullPageWrapper>
  );
};

const useDashboardStyles = makeStyles((theme: Theme) =>
  createStyles({
    box: {
      width: "65%",
      backgroundColor: theme.palette.background.paper,
    },
    avatarBackground: {
      backgroundImage: (props: StylesProps) => `url(${props.backgroundUrl})`,
    },
    avatar: {
      width: theme.spacing(8),
      height: theme.spacing(8),
    },
  })
);

interface StylesProps {
  backgroundUrl: string;
}

const RealDashboard: React.FC = () => {
  const { state } = useContext(UserContextInstance);
  const styles = useDashboardStyles({
    backgroundUrl: state.photo_url,
  });
  const [panelState, panelStateSetter] = useState(false);
  return (
    <SwipeableDrawer
      classes={{ paper: styles.box }}
      open={panelState}
      onClose={() => panelStateSetter(false)}
      onOpen={() => panelStateSetter(true)}
      anchor={"left"}
    >
      <div className="panel-user-container">
        <div className={`${styles.avatarBackground} panel-user-background`} />
        <div className="panel-user-content">
          <Avatar
            src={state && state.photo_url}
            className={`${styles.avatar} user-avatar`}
          />
          <div className="panel-user-info-container">
            {[state.first_name, `@${state.username}`].map((x) => (
              <div className="user-info-row">{x}</div>
            ))}
          </div>
        </div>
      </div>
      panel content menu
    </SwipeableDrawer>
  );
};

export default Dashboard;
