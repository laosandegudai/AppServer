import * as fakeProjects from "./fake";
import { request } from "../client";
export function getFolders(fake = true) {
  if (fake) {
    return fakeProjects.getFolders();
  }
}

export function getAllProjectsList(fake = true, filter) {
  if (fake) {
    return fakeProjects.getAllProjects(filter.page);
  }
  const options = {
    method: "get",
    url: `/project/filter`,
  };
  return request(options);
}

export function getMyProjectsList(fake = true) {
  if (fake) {
    return fakeProjects.getMyProjects();
  }
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
