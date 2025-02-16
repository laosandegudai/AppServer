import { request, setWithCredentialsStatus } from "../client";

export function login(userName, passwordHash) {
  const data = {
    userName,
    passwordHash,
  };

  return request({
    method: "post",
    url: "/authentication.json",
    data,
  });
}

export function thirdPartyLogin(SerializedProfile) {
  return request({
    method: "post",
    url: "authentication.json",
    data: { SerializedProfile },
  });
}

export function logout() {
  return request({
    method: "post",
    url: "/authentication/logout",
  });
}

export function checkConfirmLink(data) {
  return request({
    method: "post",
    url: "/authentication/confirm.json",
    data,
  });
}

export function checkIsAuthenticated() {
  return request({
    method: "get",
    url: "/authentication",
    withCredentials: true,
  }).then((state) => {
    setWithCredentialsStatus(state);
    return state;
  });
}

export function loginWithTfaCode(userName, passwordHash, code) {
  const data = {
    userName,
    passwordHash,
    code,
  };

  return request({
    method: "post",
    url: `/authentication/${code}`,
    data,
  });
}
