import React, { useEffect, useState } from "react";
import { observer, inject } from "mobx-react";

let loadTimeout = null;
const withLoader = (WrappedComponent) => (Loader) => {
  const withLoader = (props) => {
    const { tReady, isLoaded, isLoading } = props;
    const [inLoad, setInLoad] = useState(true);

    const cleanTimer = () => {
      loadTimeout && clearTimeout(loadTimeout);
      loadTimeout = null;
    };

    useEffect(() => {
      if (isLoading) {
        cleanTimer();
        loadTimeout = setTimeout(() => {
          setInLoad(true);
        }, 500);
      } else {
        cleanTimer();

        setInLoad(false);
      }

      return () => {
        cleanTimer();
      };
    }, [isLoading]);

    return !isLoaded || inLoad || !tReady ? (
      Loader
    ) : (
      <WrappedComponent {...props} />
    );
  };

  return inject(({ auth, crmStore }) => {
    const { isLoaded } = auth;
    const { isLoading } = crmStore;
    return {
      isLoaded,
      isLoading,
    };
  })(observer(withLoader));
};

export default withLoader;
