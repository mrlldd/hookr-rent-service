import React, { useContext, useState } from "react";
import "./Settings.css";
import { createStyles, List, Theme } from "@material-ui/core";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import Divider from "@material-ui/core/Divider";
import Collapse from "@material-ui/core/Collapse";
import { Themes } from "../../core/themes";
import Icon from "@material-ui/core/Icon";
import FormControl from "@material-ui/core/FormControl";
import RadioGroup from "@material-ui/core/RadioGroup";
import { AppThemeContext } from "../../context/themes/app-theme-context";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Radio from "@material-ui/core/Radio";
import makeStyles from "@material-ui/core/styles/makeStyles";
import Box from "@material-ui/core/Box";

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    nested: {
      paddingLeft: theme.spacing(4),
    },
    list: {
      color: theme.palette.text.primary,
    },
  })
);

function formatLabel(label: string) {
  return label
    .replace("-", " ")
    .split(" ")
    .map((x) => x[0].toUpperCase() + x.substring(1))
    .reduce((prev, next) => prev + " " + next);
}

const Settings: React.FC = () => {
  const styles = useStyles();
  const [themeSelectorOpened, themeSelectorOpenedSetter] = useState(false);
  const { theme, setTheme } = useContext(AppThemeContext);
  const themeChangeHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
    setTheme(event.target.value as Themes);
  };
  return (
    <div className="Settings" data-testid="Settings">
      <Box>
        <List className={styles.list}>
          <ListItem
            button
            onClick={() => themeSelectorOpenedSetter(!themeSelectorOpened)}
          >
            <ListItemText primary="Theme" />
            <Icon>{themeSelectorOpened ? "expand_less" : "expand_more"}</Icon>
          </ListItem>
          <Collapse in={themeSelectorOpened}>
            <FormControl color="primary">
              <RadioGroup value={theme} onChange={themeChangeHandler}>
                {Object.values(Themes).map((x) => (
                  <FormControlLabel
                    key={x}
                    value={x}
                    control={<Radio color={"primary"} />}
                    label={formatLabel(x)}
                    className={styles.nested}
                  />
                ))}
              </RadioGroup>
            </FormControl>
          </Collapse>
          <Divider />
        </List>
      </Box>
    </div>
  );
};

export default Settings;
