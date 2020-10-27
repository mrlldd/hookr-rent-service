import React from "react";
import "./FullPageWrapper.css";

const FullPageWrapper: React.FC<React.PropsWithChildren<
  React.HTMLAttributes<never>
>> = (props: React.PropsWithChildren<React.HTMLAttributes<never>>) => (
  <div
    className={`${props.className} FullPageWrapper`}
    data-testid="FullPageWrapper"
  >
    {props.children}
  </div>
);

export default FullPageWrapper;
