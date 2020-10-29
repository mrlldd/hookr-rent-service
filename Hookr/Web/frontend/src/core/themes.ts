import { Theme } from "@material-ui/core";
import createMuiTheme from "@material-ui/core/styles/createMuiTheme";

export enum Themes {
  DarkBlue = "dark-blue",
  Dark = "dark",
  White = "white",
}

export const defaultTheme = Themes.DarkBlue;

export function getTheme(key: Themes): Theme {
  switch (key) {
    case Themes.DarkBlue:
      return darkBlueTheme;
    case Themes.Dark:
      return darkTheme;
    case Themes.White:
      return whiteTheme;
    default: {
      throw new Error("Not found theme: " + key);
    }
  }
}

const darkBlueTheme = createMuiTheme({
  palette: {
    background: {
      default: "rgb(12,46,72)",
      paper: "rgb(13,28,55)",
    },
    type: "dark",
  },
});

const darkTheme = createMuiTheme({
  palette: {
    /*background: {
            default: "rgb(55,55,55)"
        },*/
    type: "dark",
  },
});

const whiteTheme = createMuiTheme({
  palette: {
    /*background: {
            default: "rgb(255,255,255)"
        },*/
    type: "light",
  },
});
