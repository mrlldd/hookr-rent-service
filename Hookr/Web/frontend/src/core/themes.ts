import { Theme, ThemeOptions } from "@material-ui/core";
import createMuiTheme from "@material-ui/core/styles/createMuiTheme";

export enum Themes {
  DarkBlue = "dark-blue",
  Dark = "dark",
  Light = "light",
}

export const defaultTheme = Themes.Light;

function getOptions(key: Themes): ThemeOptions {
  switch (key) {
    case Themes.DarkBlue:
      return darkBlueTheme;
    case Themes.Dark:
      return darkTheme;
    case Themes.Light:
      return whiteTheme;
    default: {
      throw new Error("Not found theme: " + key);
    }
  }
}

export function getTheme(key: Themes): Theme {
  return createMuiTheme(getOptions(key));
}

const darkBlueTheme: ThemeOptions = {
  palette: {
    background: {
      default: "rgb(12,46,72)",
      paper: "rgb(13,28,55)",
    },
    type: "dark",
  },
};

const darkTheme: ThemeOptions = {
  palette: {
    /*background: {
            default: "rgb(55,55,55)"
        },*/
    type: "dark",
  },
};

const whiteTheme: ThemeOptions = {
  palette: {
    /*background: {
            default: "rgb(255,255,255)"
        },*/
    type: "light",
  },
};
