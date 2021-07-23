import * as fakeProjects from "./fake";
import { request } from "../client";
import ProjectsFilter from "./projectsFilter";
export function getFolders(fake = true) {
  if (fake) {
    return fakeProjects.getFolders();
  }
}

export function getAllProjectsList(fake = false, filter) {
  /*   if (fake) {
    return fakeProjects.getAllProjects(filter.page);
  } */
  const params =
    filter && filter instanceof ProjectsFilter
      ? `filter?${filter.toApiUrlParams()}`
      : "";

  console.log(params);

  const options = {
    method: "get",
    url: `/project/${params}.json`,
    old: true,
  };
  return request(options);
}

export function getMyProjectsList(fake = false, filter) {
  const params =
    filter && filter instanceof ProjectsFilter
      ? `filter?${filter.toApiUrlParams()}`
      : "";

  console.log(params);

  const options = {
    method: "get",
    url: `/project/${params}.json`,
    old: true,
  };
  return request(options);
}

export function getFollowedProjectsList(fake = true) {
  if (fake) {
    return fakeProjects.getFollowedProjects();
  }
}

export function getActiveProjectsList(fake = true) {
  if (fake) {
    return fakeProjects.getActiveProjects();
  }
}

export function getPausedProjectsList(fake = true) {
  if (fake) {
    return fakeProjects.getPausedProjects();
  }
}

export function getClosedProjectsList(fake = true) {
  if (fake) {
    return fakeProjects.getClosedProjects();
  }
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
