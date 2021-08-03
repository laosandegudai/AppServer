import { ProjectOptionStatus, TaskOptionStatus } from "../constants";

export const getProjectStatus = (project) => {
  if (project.status === ProjectOptionStatus.Active) {
    return "active";
  } else if (project.status === ProjectOptionStatus.Paused) {
    return "paused";
  } else if (project.status === ProjectOptionStatus.Closed) {
    return "closed";
  } else {
    return "unknown";
  }
};

export const getTaskStatus = (task) => {
  if (task.status === TaskOptionStatus.Active) {
    return "active";
  } else if (task.status === TaskOptionStatus.Closed) {
    return "closed";
  } else {
    return "unknown";
  }
};
