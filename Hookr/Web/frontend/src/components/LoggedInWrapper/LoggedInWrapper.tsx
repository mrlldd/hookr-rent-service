import React, { useContext, useState } from "react";
import "./LoggedInWrapper.css";
import FullPageWrapper from "../FullPageWrapper/FullPageWrapper";

import makeStyles from "@material-ui/core/styles/makeStyles";
import { createStyles, SwipeableDrawer, Theme } from "@material-ui/core";
import {
  UserContextInstance,
  userInitialState,
} from "../../context/user/user-context-instance";
import Avatar from "@material-ui/core/Avatar";
import Divider from "@material-ui/core/Divider";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import Icon from "@material-ui/core/Icon";
import { Link, Route, Switch } from "react-router-dom";
import { dashboardRoute, settingsRoute } from "../../App";
import Settings from "../Settings/Settings";
import { clearSession } from "../../context/local-storage-utils";

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

function nested(route: string): string {
  return dashboardRoute + route;
}

const LoggedInWrapper: React.FC = () => {
  return (
    <FullPageWrapper className="LoggedInWrapper" data-testid="LoggedInWrapper">
      <WithSidePanel />
      <Route path={nested(settingsRoute)} component={Settings} />
    </FullPageWrapper>
  );
};

const WithSidePanel: React.FC = () => {
  const { state } = useContext(UserContextInstance);
  const styles = useDashboardStyles({
    backgroundUrl: state.photo_url,
  });
  const [panelState, panelStateSetter] = useState(false);
  const { dispatch } = useContext(UserContextInstance);
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
              <div key={x} className="user-info-row">
                {x}
              </div>
            ))}
          </div>
        </div>
      </div>
      <List>
        <ListItem
          key={settingsRoute}
          button
          onClick={() => panelStateSetter(false)}
          component={Link}
          to={nested(settingsRoute)}
        >
          <ListItemIcon>
            <Icon>settings</Icon>
          </ListItemIcon>
          <ListItemText primary="Settings" />
        </ListItem>
        <Divider />
        <ListItem
          key={"log out"}
          button
          onClick={() => {
            clearSession();
            dispatch(userInitialState);
          }}
        >
          <ListItemIcon>
            <Icon>exit_to_app</Icon>
          </ListItemIcon>
          <ListItemText primary="Log out" />
        </ListItem>
      </List>
    </SwipeableDrawer>
  );
};

export default LoggedInWrapper;
