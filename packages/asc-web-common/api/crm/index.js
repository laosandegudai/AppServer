import { data } from "./fake";
import CrmFilter from "./filter";
import { request } from "../client";

export async function getFoldersTree() {
  return Promise.resolve(data);
}

export function getContactsList(filter = Filter.getDefault()) {
  const params =
    filter && filter instanceof CrmFilter
      ? `/filter?${filter.toApiUrlParams()}`
      : "";

  const options = {
    method: "get",
    url: `/crm/contact${params}`,
  };

  return request(options);
}

export function getFilterContactsTypes() {
  const options = {
    method: "get",
    url: "/crm/contact/type",
  };

  return request(options);
}

export function getFilterTempLevels() {
  const options = {
    method: "get",
    url: "/crm/contact/status",
  };

  return request(options);
}
