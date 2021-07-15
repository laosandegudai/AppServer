import { RowProjectOptionStatus } from "../constants";

export const getProjectStatus = (project) => {
  if (project.status === RowProjectOptionStatus.Active) {
    return "active";
  } else if (project.status === RowProjectOptionStatus.Paused) {
    return "paused";
  } else if (project.status === RowProjectOptionStatus.Closed) {
    return "closed";
  } else {
    return "unknown";
  }
};
