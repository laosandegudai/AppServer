import { inject, observer } from "mobx-react";
import EmptyScreenContainer from "@appserver/components/empty-screen-container";
import IconButton from "@appserver/components/icon-button";
import Link from "@appserver/components/link";
import Box from "@appserver/components/box";
import Grid from "@appserver/components/grid";
import React from "react";
import { useTranslation } from "react-i18next";
import api from "@appserver/common/api";
const { ProjectsFilter, TasksFilter } = api;
const EmptyScreen = ({ filter, fetchProjects, fetchTasks, setIsLoading }) => {
  const { t } = useTranslation(["Home", "Common"]);

  const onResetFilter = () => {
    setIsLoading(true);
    if (filter instanceof ProjectsFilter) {
      const newFilter = ProjectsFilter.getDefault();
      fetchProjects(newFilter).finally(() => setIsLoading(false));
      // .catch((err) => toastr.error(err))
    }

    if (filter instanceof TasksFilter) {
      const newFilter = TasksFilter.getDefault();
      fetchTasks(newFilter).finally(() => setIsLoading(false));
    }
  };

  const title =
    filter instanceof ProjectsFilter
      ? t("Home:EmptyProjectTitle")
      : filter instanceof TasksFilter
      ? t("EmptyTaskTitle")
      : null;

  const description =
    filter instanceof ProjectsFilter
      ? t("EmptyProjectDescription")
      : filter instanceof TasksFilter
      ? t("EmptyTaskDescription")
      : null;

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
          {false ? null : (
            <>
              <Box>
                <IconButton
                  className="empty-folder_container-icon"
                  size="12"
                  iconName="/static/images/cross.react.svg"
                  onClick={onResetFilter}
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
          )}
        </Grid>
      }
    />
  );
};

export default inject(
  ({ projectsStore, projectsFilterStore, tasksFilterStore }) => {
    const { filter, setIsLoading } = projectsStore;
    return {
      setIsLoading,
      filter,
      fetchProjects: projectsFilterStore.fetchProjects,
      fetchTasks: tasksFilterStore.fetchTasks,
    };
  }
)(observer(EmptyScreen));
