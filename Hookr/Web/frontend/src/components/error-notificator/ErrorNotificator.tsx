import React, { useState } from "react";
import "./ErrorNotificator.css";
import { Alert } from "@material-ui/lab";
import { Snackbar } from "@material-ui/core";
import Slide from "@material-ui/core/Slide";
import { store } from "../../store/store";
import { ErrorNotificatorState } from "../../store/error-notificator/error-notificator-reducer";
import { connect } from "react-redux";
import { RootState } from "../../store/root-reducer";
import {
  ChangeNotificatorSizeAction,
  HideNotificatorAction,
  ResetNotificatorAction,
} from "../../store/error-notificator/error-notificator-actions";
import ClickAwayListener from "@material-ui/core/ClickAwayListener";

type Props = ErrorNotificatorState;

const maxLength = 20;

function withValidatedLength(
  source: string,
  needsToBeValidated: boolean
): string {
  return needsToBeValidated
    ? source.substring(0, maxLength).concat("...")
    : source;
}

const transitionDuration = 1000;
const autohideDurationDefault = 2000;
const ErrorNotificator: React.FC = (
  props: React.PropsWithChildren<any> & Props
) => {
  let [duration, setDuration] = useState(autohideDurationDefault);
  return (
    <div className="ErrorNotificator" data-testid="ErrorNotificator">
      {props.children}
      <ClickAwayListener
        onClickAway={() => store.dispatch(HideNotificatorAction)}
      >
        <Snackbar
          open={props.opened}
          anchorOrigin={{
            horizontal: "center",
            vertical: "top",
          }}
          transitionDuration={transitionDuration}
          autoHideDuration={duration}
          onClose={() => store.dispatch(HideNotificatorAction)}
          onTransitionEnd={() => {
            if (!store.getState().errorNotificator.opened) {
              store.dispatch(ResetNotificatorAction);
              setDuration(autohideDurationDefault);
            }
          }}
          TransitionComponent={Slide}
        >
          <Alert
            severity="error"
            variant="filled"
            onClick={() => {
              store.dispatch(ChangeNotificatorSizeAction);
              setDuration(duration * 2);
            }}
          >
            <span
              style={{
                textDecoration: "underline",
              }}
            >
              {props.type}
            </span>
            {withValidatedLength(
              `: ${props.description} (${props.traceId})`,
              !props.fullSize
            )}
          </Alert>
        </Snackbar>
      </ClickAwayListener>
    </div>
  );
};

function mapper(state: RootState): Props {
  return state.errorNotificator;
}

export default connect(mapper)(ErrorNotificator);
