import { defaultTheme, Themes } from "../../core/themes";
import { createContext, Dispatch } from "react";

interface ThemeContextState {
  theme: Themes;
  setTheme: Dispatch<Themes>;
}

export const AppThemeContext = createContext<ThemeContextState>({
  theme: defaultTheme,
  setTheme: () => null,
});
