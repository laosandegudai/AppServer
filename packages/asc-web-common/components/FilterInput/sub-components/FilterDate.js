import React from "React";
import DatePicker from "@appserver/components/date-picker";
import moment from "moment";

export const FilterDate = () => {
  const now = moment();
  console.log(now);
  const fromLabel = "From:";
  const toLabel = "To:";

  return (
    <>
      {fromLabel}
      <DatePicker
        calendarHeaderContent="Select Date"
        calendarSize="base"
        locale="en"
        onChange={(data) => console.log(data)}
        maxDate={new Date("2021-12-31T19:00:00.000Z")}
        minDate={new Date("2021-07-20T10:00:00.000Z")}
        openToDate={new Date("2021-07-20T08:28:42.246Z")}
        selectedDate={new Date("2021-07-20T10:00:00.000Z")}
        className="styled-datepicker"
      />
      {toLabel}
      <DatePicker
        calendarHeaderContent="Select Date"
        calendarSize="big"
        locale="en"
        maxDate={new Date("2021-12-31T19:00:00.000Z")}
        minDate={new Date("2021-07-21T19:00:00.000Z")}
        openToDate={new Date("2021-07-20T08:28:42.246Z")}
        selectedDate={new Date("2021-07-21T19:00:00.000Z")}
        className="styled-datepicker"
      />
    </>
  );
};
