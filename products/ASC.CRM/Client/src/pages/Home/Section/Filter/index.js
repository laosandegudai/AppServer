import React, { useState } from "react";
import find from "lodash/find";
import result from "lodash/result";
import Loaders from "@appserver/common/components/Loaders";
import { ContactsFilterType } from "@appserver/common/constants";
import { inject, observer } from "mobx-react";
import { withTranslation } from "react-i18next";
import { isMobileOnly } from "react-device-detect";
import FilterInput from "@appserver/common/components/FilterInput";
import Checkbox from "@appserver/components/checkbox";
import { withLayoutSize } from "@appserver/common/utils";
import { withRouter } from "react-router";
import moment from "moment";

const SectionFilterContent = ({
  sectionWidth,
  isLoaded,
  tReady,
  t,
  filter,
  getContactsList,
  setIsLoading,
  customNames,
  user,
}) => {
  const [isCheckedTag, setIsCheckedTag] = useState({
    Client: false,
    Staff: false,
    Provider: false,
    "Potential client": false,
  });
  const [renderId, setRenderId] = useState(0);

  const getAuthorType = (filterValues) => {
    const authorType = result(
      find(filterValues, (value) => {
        return value.group === "filter-author";
      }),
      "key"
    );

    return authorType
      ? authorType === "filter-no-author"
        ? ContactsFilterType.NoAuthor
        : authorType
      : null;
  };

  const getSelectedItem = (filterValues, type) => {
    const selectedItem = filterValues.find((item) => item.key === type);
    return selectedItem || null;
  };

  const getAccessibilityType = (filterValues) => {
    const isShared = result(
      find(filterValues, (value) => {
        return value.group === "filter-access";
      }),
      "key"
    );

    return isShared
      ? isShared === "filter-access-public"
        ? "true"
        : "false"
      : null;
  };

  const getTemperatureType = (filterValues) => {
    const contactStage = result(
      find(filterValues, (value) => {
        return value.group === "filter-other-temperature";
      }),
      "key"
    );
    return contactStage ? contactStage === "not-specified" && "0" : null;
  };

  const getContactType = (filterValues) => {
    const contactType = result(
      find(filterValues, (value) => {
        return value.group === "filter-other-contact-type";
      }),
      "key"
    );

    return contactType ? contactType === "not-specified" && "0" : null;
  };

  const onChangeTagCheckbox = (name) => {
    setIsCheckedTag({ ...isCheckedTag, [name]: !isCheckedTag[name] });
    const newIsCheckedTag = { ...isCheckedTag, [name]: !isCheckedTag[name] };

    const checkedTags = Object.keys(newIsCheckedTag).filter(
      (tag) => newIsCheckedTag[tag]
    );

    setRenderId(renderId + 1);

    const newFilter = filter.clone();
    newFilter.tags = checkedTags;

    setIsLoading(true);
    getContactsList(newFilter).finally(() => setIsLoading(false));
  };

  const getDateType = (filterValues) => {
    const lastMonthStart = moment()
      .subtract(1, "months")
      .date(1)
      .set({ hour: 0, minute: 0, second: 0, millisecond: 0 })
      .format();

    const lastMonthEnd = moment()
      .subtract(1, "months")
      .endOf("month")
      .set({ hour: 0, minute: 0, second: 0, millisecond: 0 })
      .format();

    const yesterday = moment()
      .subtract(1, "days")
      .set({ hour: 0, minute: 0, second: 0, millisecond: 0 })
      .format();

    const today = moment()
      .set({ hour: 0, minute: 0, second: 0, millisecond: 0 })
      .format();
    const thisMonth = moment()
      .startOf("month")
      .set({ hour: 0, minute: 0, second: 0, millisecond: 0 })
      .format();

    const date = result(
      find(filterValues, (value) => {
        return value.group === "filter-creation-date";
      }),
      "key"
    );

    switch (date) {
      case "filter-last-month":
        return {
          fromDate: lastMonthStart,
          toDate: lastMonthEnd,
        };
      case "filter-yesterday":
        return {
          fromDate: yesterday,
          toDate: yesterday,
        };
      case "filter-today":
        return {
          fromDate: today,
          toDate: today,
        };
      case "filter-this-month":
        return { fromDate: thisMonth, toDate: today };
      default:
        return { fromDate: null, toDate: null };
    }
  };

  const getContactListViewType = (filterValues) => {
    const contactListView = result(
      find(filterValues, (value) => {
        return value.group === "filter-show";
      }),
      "key"
    );

    return contactListView
      ? contactListView === "filter-show-company"
        ? "company"
        : contactListView === "filter-show-person"
        ? "person"
        : "withopportunity"
      : null;
  };

  const getSelectedFilterData = () => {
    const selectedFilterData = {
      filterValues: [],
      sortDirection: filter.sortOrder === "ascending" ? "asc" : "desc",
      sortId: filter.sortBy,
    };

    selectedFilterData.inputValue = filter.search;

    if (filter.authorType) {
      selectedFilterData.filterValues.push({
        key: `${filter.authorType}`,
        group: "filter-author",
      });
    }

    if (filter.isShared) {
      selectedFilterData.filterValues.push({
        key: `${filter.isShared}`,
        group: "filter-access",
      });
    }

    selectedFilterData.filterValues.push({
      key: `${filter.contactStage}`,
      group: "filter-other-temperature",
    });

    selectedFilterData.filterValues.push({
      key: `${filter.contactType}`,
      group: "filter-other-contact-type",
    });

    if (filter.fromDate) {
      selectedFilterData.filterValues.push({
        key: `${filter.fromDate}`,
        group: "filter-creation-date",
      });
    }

    if (filter.toDate) {
      selectedFilterData.filterValues.push({
        key: `${filter.fromDate}`,
        group: "filter-creation-date",
      });
    }

    if (filter.contactListView) {
      selectedFilterData.filterValues.push({
        key: `${filter.contactListView}`,
        group: "filter-show",
      });
    }

    if (filter.tags) {
      selectedFilterData.filterValues.push({
        key: `${filter.tags}`,
        group: "filter-other-tag-type",
      });
    }

    return selectedFilterData;
  };

  const selectedFilterData = getSelectedFilterData();

  const onFilter = (data) => {
    const authorType = getAuthorType(data.filterValues);
    const isShared = getAccessibilityType(data.filterValues);
    const contactStage = getTemperatureType(data.filterValues);
    const contactType = getContactType(data.filterValues);
    const contactListView = getContactListViewType(data.filterValues);
    const { fromDate, toDate } = getDateType(data.filterValues);
    const sortBy = data.sortId;
    const sortOrder =
      data.sortDirection === "desc" ? "descending" : "ascending";
    const search = data.inputValue || "";

    const selectedItem = authorType
      ? getSelectedItem(data.filterValues, authorType)
      : null;
    const selectedFilterItem = {};
    if (selectedItem) {
      selectedFilterItem.key = selectedItem.selectedItem.key;
      selectedFilterItem.label = selectedItem.selectedItem.label;
      selectedFilterItem.type = selectedItem.typeSelector;
    }

    const newFilter = filter.clone();
    newFilter.page = 0;
    newFilter.sortBy = sortBy;
    newFilter.sortOrder = sortOrder;
    newFilter.authorType = authorType;
    newFilter.isShared = isShared;
    newFilter.contactListView = contactListView;
    newFilter.search = search;
    newFilter.fromDate = fromDate;
    newFilter.toDate = toDate;
    newFilter.contactStage = contactStage;
    newFilter.contactType = contactType;
    newFilter.selectedItem = selectedFilterItem;

    setIsLoading(true);
    getContactsList(newFilter).finally(() => setIsLoading(false));
  };

  const getData = () => {
    const { groupsCaption } = customNames;
    const { selectedItem } = filter;

    const groupOptions = [
      {
        key: "not-specified",
        inSubgroup: true,
        group: "filter-other-temperature",
        label: t("NotSpecified"),
      },
      {
        key: "cold-temperature",
        inSubgroup: true,
        group: "filter-other-temperature",
        label: t("ColdTemperature"),
      },
      {
        key: "warm-temperature",
        inSubgroup: true,
        group: "filter-other-temperature",
        label: t("WarmTemperature"),
      },
      {
        key: "hot-temperature",
        inSubgroup: true,
        group: "filter-other-temperature",
        label: t("HotTemperature"),
      },
      {
        key: "no-category",
        inSubgroup: true,
        group: "filter-other-contact-type",
        label: t("NoCategorySpecified"),
      },
      {
        key: "client-type",
        inSubgroup: true,
        group: "filter-other-contact-type",
        label: t("Client"),
      },
      {
        key: "staff-type",
        inSubgroup: true,
        group: "filter-other-contact-type",
        label: t("Staff"),
      },
      {
        key: "provider-type",
        inSubgroup: true,
        group: "filter-other-contact-type",
        label: t("Provider"),
      },
      {
        key: "potential-client-type",
        inSubgroup: true,
        group: "filter-other-contact-type",
        label: t("PotentialClient"),
      },
      {
        key: "client-tag-type",
        inSubgroup: true,
        group: "filter-other-tag-type",
        children: [
          <Checkbox
            key="client-checkbox"
            label={t("Client")}
            isChecked={isCheckedTag["Client"]}
            onChange={() => onChangeTagCheckbox("Client")}
          />,
        ],
      },
      {
        key: "staff-tag-type",
        inSubgroup: true,
        group: "filter-other-tag-type",
        children: [
          <Checkbox
            key="staff-checkbox"
            label={t("Staff")}
            isChecked={isCheckedTag["Staff"]}
            onChange={() => onChangeTagCheckbox("Staff")}
          />,
        ],
      },
      {
        key: "provider-tag-type",
        inSubgroup: true,
        group: "filter-other-tag-type",
        children: [
          <Checkbox
            key="provider-checkbox"
            label={t("Provider")}
            isChecked={isCheckedTag["Provider"]}
            onChange={() => onChangeTagCheckbox("Provider")}
          />,
        ],
      },
      {
        key: "potential-client-tag-type",
        inSubgroup: true,
        group: "filter-other-tag-type",
        children: [
          <Checkbox
            key="potential-client-checkbox"
            label={t("PotentialClient")}
            isChecked={isCheckedTag["Potential client"]}
            onChange={() => onChangeTagCheckbox("Potential client")}
          />,
        ],
      },
    ];

    const options = [
      {
        key: "filter-author",
        group: "filter-author",
        label: t("Manager"),
        isHeader: true,
      },
      {
        key: "user",
        group: "filter-author",
        label: t("My"),
        isSelector: true,
        defaultOptionLabel: t("Common:MeLabel"),
        defaultSelectLabel: t("Common:Select"),
        groupsCaption,
        defaultOption: user,
        selectedItem,
      },
      {
        key: "filter-no-author",
        group: "filter-author",
        label: t("NoContactManager"),
      },
      {
        key: "user-custom",
        isSelector: true,
        defaultOptionLabel: t("Common:MeLabel"),
        defaultSelectLabel: t("Common:Select"),
        groupsCaption,
        defaultOption: user,
        group: "filter-author",
        label: t("Custom"),
        selectedItem,
      },
      {
        key: "filter-access",
        group: "filter-access",
        label: t("Accessibility"),
        isRowHeader: true,
      },
      {
        key: "filter-access-public",
        group: "filter-access",
        label: t("Public"),
      },
      {
        key: "filter-access-restricted",
        group: "filter-access",
        label: t("Restricted"),
      },
      {
        key: "filter-other",
        group: "filter-other",
        label: t("Common:Other"),
        isRowHeader: true,
      },
      {
        key: "filter-other-temperature-level",
        group: "filter-other",
        subgroup: "filter-other-temperature",
        label: t("TemperatureLevel"),
      },
      {
        key: "filter-other-contact-type",
        group: "filter-other",
        subgroup: "filter-other-contact-type",
        label: t("ContactType"),
      },
      {
        key: "filter-other-with-tag",
        group: "filter-other",
        subgroup: "filter-other-tag-type",
        label: t("WithTag"),
      },
      ...groupOptions,
    ];

    const filterOptions = [
      ...options,
      {
        key: "filter-creation-date",
        group: "filter-creation-date",
        label: t("CreationDate"),
        isHeader: true,
      },
      {
        key: "filter-last-month",
        group: "filter-creation-date",
        label: t("LastMonth"),
      },
      {
        key: "filter-yesterday",
        group: "filter-creation-date",
        label: t("Yesterday"),
      },
      {
        key: "filter-today",
        group: "filter-creation-date",
        label: t("Today"),
      },
      {
        key: "filter-this-month",
        group: "filter-creation-date",
        label: t("ThisMonth"),
      },
      {
        key: "filter-show",
        group: "filter-show",
        label: t("Show"),
        isRowHeader: true,
      },
      {
        key: "filter-show-company",
        group: "filter-show",
        label: t("Companies"),
      },
      {
        key: "filter-show-person",
        group: "filter-show",
        label: t("Persons"),
      },
      {
        key: "filter-show-with-opportunity",
        group: "filter-show",
        label: t("WithOpportunities"),
      },
    ];

    return filterOptions;
  };

  const getSortData = () => {
    const commonOptions = [
      { key: "created", label: t("CreationDate"), default: true },
      { key: "displayname", label: t("Common:ByTitle"), default: true },
      {
        key: "contacttype",
        label: t("ByTemperatureLevel"),
        default: true,
      },
      { key: "history", label: t("ByHistory"), default: true },
    ];

    const viewSettings = [
      {
        key: "row",
        label: t("Common:ViewList"),
        isSetting: true,
        default: true,
      },
      {
        key: "tile",
        label: t("Common:ViewTiles"),
        isSetting: true,
        default: true,
      },
    ];

    return window.innerWidth < 460
      ? [...commonOptions, ...viewSettings]
      : commonOptions;
  };

  const filterColumnCount =
    window.innerWidth < 500 ? {} : { filterColumnCount: 2 };

  return isLoaded && tReady ? (
    <FilterInput
      sectionWidth={sectionWidth}
      getFilterData={getData}
      getSortData={getSortData}
      selectedFilterData={selectedFilterData}
      onFilter={onFilter}
      directionAscLabel={t("Common:DirectionAscLabel")}
      directionDescLabel={t("Common:DirectionDescLabel")}
      placeholder={t("Common:Search")}
      {...filterColumnCount}
      contextMenuHeader={t("Common:AddFilter")}
      isMobile={isMobileOnly}
      id={renderId.toString()}
    />
  ) : (
    <Loaders.Filter />
  );
};

export default withRouter(
  inject(({ auth, contactsStore, crmStore }) => {
    const { isLoaded } = auth;
    const { filter } = contactsStore.filterStore;
    const { getContactsList } = contactsStore;
    const { customNames } = auth.settingsStore;
    const { user } = auth.userStore;
    const { setIsLoading } = crmStore;

    return {
      isLoaded,
      filter,
      getContactsList,

      customNames,
      user,
      selectedItem: filter.selectedItem,
      setIsLoading,
    };
  })(
    withLayoutSize(
      withTranslation(["Home", "Common"])(observer(SectionFilterContent))
    )
  )
);
