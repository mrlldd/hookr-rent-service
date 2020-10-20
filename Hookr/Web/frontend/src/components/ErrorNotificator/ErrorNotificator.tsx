import React, { ReactNode, useContext, useEffect, useState } from "react";
import "./ErrorNotificator.css";
import { Alert } from "@material-ui/lab";
import { Snackbar } from "@material-ui/core";
import Slide from "@material-ui/core/Slide";
import ClickAwayListener from "@material-ui/core/ClickAwayListener";
import { ErrorNotificatorContextInstance } from "../../context/error-notificator/error-notificator-context-instance";

interface ErrorNotificatorState {
  opened: boolean;
  fullSize: boolean;
}

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
const autohideDurationDefault = 4000;

const ErrorNotificatorContextConsumer: React.FC = () => {
  const [duration, setDuration] = useState(autohideDurationDefault);
  const [componentState, setter] = useState<ErrorNotificatorState>({
    fullSize: false,
    opened: false,
  });
  const { state } = useContext(ErrorNotificatorContextInstance);
  useEffect(
    () =>
      setter({
        ...componentState,
        opened: Boolean(state),
      }),
    [state]
  );
  return (
    <div className="ErrorNotificator" data-testid="ErrorNotificator">
      <ClickAwayListener
        onClickAway={() => setter({ ...componentState, opened: false })}
      >
        <Snackbar
          open={componentState.opened}
          anchorOrigin={{
            horizontal: "center",
            vertical: "top",
          }}
          transitionDuration={transitionDuration}
          autoHideDuration={duration}
          onClose={() =>
            setter({
              ...componentState,
              opened: false,
            })
          }
          onTransitionEnd={() => {
            if (!componentState.opened) {
              setter({
                fullSize: false,
                opened: false,
              });
              setDuration(autohideDurationDefault);
            }
          }}
          TransitionComponent={Slide}
        >
          <Alert
            severity="error"
            variant="filled"
            onClick={() => {
              setter({ ...componentState, fullSize: !componentState.fullSize });
              setDuration(duration * 2);
            }}
          >
            <span
              style={{
                textDecoration: "underline",
              }}
            >
              {state && state.type}
            </span>
            {withValidatedLength(
              `: ${state && state.description} (${state && state.traceId})`,
              !componentState.fullSize
            )}
          </Alert>
        </Snackbar>
      </ClickAwayListener>
    </div>
  );
};

const ErrorNotificator: React.FC = (
  props: React.PropsWithChildren<ReactNode>
) => {
  return (
    <div>
      <ErrorNotificatorContextConsumer />
      {props.children}
    </div>
  );
};

export default ErrorNotificator;
