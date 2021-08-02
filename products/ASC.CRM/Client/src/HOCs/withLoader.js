import React, { useEffect, useState } from "react";
import { observer, inject } from "mobx-react";

let loadTimeout = null;
const withLoader = (WrappedComponent) => (Loader) => {
  const withLoader = (props) => {
    const { tReady, isLoaded, isLoading, firstLoad } = props;
    const [inLoad, setInLoad] = useState(true);

    const cleanTimer = () => {
      loadTimeout && clearTimeout(loadTimeout);
      loadTimeout = null;
    };

    useEffect(() => {
      if (isLoading) {
        cleanTimer();
        loadTimeout = setTimeout(() => {
          //console.log("inLoad", true);
          setInLoad(true);
        }, 500);
      } else {
        cleanTimer();
        //console.log("inLoad", false);
        setInLoad(false);
      }

      return () => {
        cleanTimer();
      };
    }, [isLoading]);

    return firstLoad || !isLoaded || inLoad || !tReady ? (
      Loader
    ) : (
      <WrappedComponent {...props} />
    );
  };

  return inject(({ auth, crmStore }) => {
    const { isLoaded } = auth;
    const { isLoading, firstLoad } = crmStore;
    return {
      isLoaded,
      isLoading,
      firstLoad,
    };
  })(observer(withLoader));
};

export default withLoader;
