import React from "react";
import classes from "./ChatUsageByDepartment.module.css";
import { VictoryPie } from "victory";

const ChatUsageByDepartment = () => {
    return (
        <div className={classes.chatUsageByDepartment}>
            <h6>Department</h6>
            <VictoryPie
                height={300}
                colorScale={["red", "orange", "gold", "cyan", "navy" ]}
                data={[
                    { x: "Cats", y: 35 },
                    { x: "Dogs", y: 40 },
                    { x: "Birds", y: 55 }
                ]}
            />
      </div>
    )
};

export default ChatUsageByDepartment;