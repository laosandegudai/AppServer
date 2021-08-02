import { inject, observer } from "mobx-react";
import EmptyScreenContainer from "@appserver/components/empty-screen-container";
import IconButton from "@appserver/components/icon-button";
import Link from "@appserver/components/link";
import Box from "@appserver/components/box";
import Grid from "@appserver/components/grid";
import React from "react";
import { useTranslation } from "react-i18next";
import api from "@appserver/common/api";
const { CrmFilter } = api;
const EmptyScreen = ({ filter, fetchContacts, setIsLoading, resetFilter }) => {
  const { t } = useTranslation(["Home", "Common"]);

  const onResetFilter = () => {
    setIsLoading(true);
    if (filter instanceof CrmFilter) {
      const newFilter = CrmFilter.getDefault();
      fetchContacts(newFilter).finally(() => setIsLoading(false));
    }
  };

  const title =
    filter instanceof CrmFilter ? t("Home:EmptyContactTitle") : null;

  const description =
    filter instanceof CrmFilter ? t("Home:EmptyContactDescription") : null;

  return (
    <EmptyScreenContainer
      imageSrc="images/empty_screen_filter.png"
      imageAlt="Empty Screen Filter image"
      headerText={title}
      descriptionText={description}
      buttons={
        <Grid
          marginProp="13px 0"
          gridColumnGap="8px"
          columnsProp={["12px 1fr"]}
        >
          {
            <>
              <Box>
                <IconButton
                  className="empty-folder_container-icon"
                  size="12"
                  onClick={onResetFilter}
                  iconName="CrossIcon"
                  isFill
                  color="#657077"
                />
              </Box>{" "}
              <Box marginProp="-4px 0 0 0">
                <Link
                  type="action"
                  isHovered={true}
                  fontWeight="600"
                  color="#555f65"
                  onClick={onResetFilter}
                >
                  {t("Common:ClearButton")}
                </Link>
              </Box>{" "}
            </>
          }
        </Grid>
      }
    />
  );
};

export default inject(({ crmStore, contactsStore, filterStore }) => {
  const { setIsLoading } = crmStore;
  const { filter } = contactsStore.filterStore;
  const { resetFilter } = filterStore;

  return {
    setIsLoading,
    filter,
    fetchContacts: contactsStore.getContactsList,
    resetFilter,
  };
})(observer(EmptyScreen));
