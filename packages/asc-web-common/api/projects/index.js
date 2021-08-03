import * as fakeProjects from "./fake";
import { request } from "../client";
import ProjectsFilter from "./projectsFilter";
import TasksFilter from "./tasksFilter";
export function getFolders(fake = true) {
  if (fake) {
    return fakeProjects.getFolders();
  }
}
export function getProjectsList(filter) {
  const params =
    filter && filter instanceof ProjectsFilter
      ? `filter?${filter.toApiUrlParams()}`
      : "";

  const options = {
    method: "get",
    url: `/project/${params}`,
    old: true,
  };
  return request(options);
}

export function getTasksList(filter) {
  const params =
    filter && filter instanceof TasksFilter
      ? `filter?${filter.toApiUrlParams()}`
      : "";

  console.log(params);

  const options = {
    method: "get",
    url: `/project/task/${params}`,
    old: true,
  };
  return request(options);
}

export function getAllTaskList(fake = true) {
  if (fake) {
    return fakeProjects.getAllTasks();
  }
}

export function getMyTaskList(fake = true) {
  if (fake) {
    return fakeProjects.getMyTasks();
  }
}
