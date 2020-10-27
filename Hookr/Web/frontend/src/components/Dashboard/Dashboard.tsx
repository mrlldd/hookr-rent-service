import React, { createRef, useContext, useState } from "react";
import "./Dashboard.css";
import FullPageWrapper from "../FullPageWrapper/FullPageWrapper";

import makeStyles from "@material-ui/core/styles/makeStyles";
import { createStyles, SwipeableDrawer } from "@material-ui/core";
import { getFromLocalStorage } from "../../context/local-storage-utils";
import { UserContextInstance } from "../../context/user/user-context-instance";

const Dashboard: React.FC = () => {
  return (
    <FullPageWrapper className="Dashboard" data-testid="Dashboard">
      <RealDashboard />
    </FullPageWrapper>
  );
};

function useDashboardStyles(photoUrl: string | undefined) {
  return makeStyles(() =>
    createStyles({
      box: {
        width: "65%",
      },
      avatarBackground: {
        background: `url(${photoUrl})`,
        backgroundSize: "cover",
        backgroundRepeat: "no-repeat",
        backgroundPosition: "center",
        minHeight: "30%",
      },
    })
  )();
}

const RealDashboard: React.FC = () => {
  const { state } = useContext(UserContextInstance);
  const styles = useDashboardStyles(state && state.photo_url);
  const [panelState, panelStateSetter] = useState(false);
  return (
    <SwipeableDrawer
      classes={{ paper: styles.box }}
      open={panelState}
      onClose={() => panelStateSetter(false)}
      onOpen={() => panelStateSetter(true)}
      anchor={"left"}
    >
      <div className={styles.avatarBackground} />
    </SwipeableDrawer>
  );
};

export default Dashboard;
