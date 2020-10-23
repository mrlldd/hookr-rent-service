import React, { useContext, useEffect, useState } from "react";
import "./ErrorNotificator.css";
import { Alert } from "@material-ui/lab";
import { Snackbar } from "@material-ui/core";
import Slide from "@material-ui/core/Slide";
import ClickAwayListener from "@material-ui/core/ClickAwayListener";
import { ErrorNotificatorContextInstance } from "../../context/error-notificator/error-notificator-context-instance";
import { useSwitchState } from "../../context/store-utils";

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
  const [opened, switchOpened] = useSwitchState(true);
  const { errorMessage } = useContext(ErrorNotificatorContextInstance);
  useEffect(switchOpened, [errorMessage]);
  const [fullSize, switchFullSize, setFullSize] = useSwitchState(false);
  const [duration, setDuration] = useState(autohideDurationDefault);
  return (
    <div className="ErrorNotificator" data-testid="ErrorNotificator">
      <ClickAwayListener onClickAway={switchOpened}>
        <Snackbar
          open={opened}
          anchorOrigin={{
            horizontal: "center",
            vertical: "top",
          }}
          transitionDuration={transitionDuration}
          autoHideDuration={duration}
          onClose={() => opened && switchOpened()}
          onTransitionEnd={() => {
            if (!opened) {
              setFullSize(false);
              setDuration(autohideDurationDefault);
            }
          }}
          TransitionComponent={Slide}
        >
          <Alert
            severity="error"
            variant="filled"
            onClick={() => {
              switchFullSize();
              setDuration(duration * 2);
            }}
          >
            <span
              style={{
                textDecoration: "underline",
              }}
            >
              {errorMessage && errorMessage.type}
            </span>
            {errorMessage &&
              withValidatedLength(
                `: ${errorMessage.description} ${
                  errorMessage.traceId ? `(${errorMessage.traceId})` : ""
                }`.trim(),
                !fullSize
              )}
          </Alert>
        </Snackbar>
      </ClickAwayListener>
    </div>
  );
};

const ErrorNotificator: React.FC = () => {
  return <ErrorNotificatorContextConsumer />;
};

export default ErrorNotificator;
