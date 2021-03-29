import React from "react";
import AutoSizer from "react-virtualized-auto-sizer";
import { FixedSizeList as List } from "react-window";
import InfiniteLoader from "react-window-infinite-loader";
import memoize from "memoize-one";
import { inject, observer } from "mobx-react";

import RowWrapper from "./RowWrapper";

const FileList = ({ items, filter, loadMoreFiles }) => {
  const createItemData = memoize((items) => ({
    items,
  }));

  const itemData = createItemData(items);

  const folderId = filter.folder;

  const isItemLoaded = (index) => !!items[index];

  const loadMoreItems = () => {
    loadMoreFiles(folderId, filter);
  };

  return (
    <AutoSizer>
      {({ height, width, style }) => (
        <InfiniteLoader
          isItemLoaded={isItemLoaded}
          itemCount={filter.total}
          loadMoreItems={loadMoreItems}
        >
          {({ onItemsRendered, ref }) => (
            <List
              className="hide-scrollbars"
              style={style}
              height={height}
              width={width}
              itemData={itemData}
              itemCount={items.length}
              itemSize={48}
              onItemsRendered={onItemsRendered}
              ref={ref}
            >
              {RowWrapper}
            </List>
          )}
        </InfiniteLoader>
      )}
    </AutoSizer>
  );
};

export default inject(({ filesStore }) => {
  const { filesList, filter } = filesStore;
  return {
    items: filesList,
    filter: filter,
    loadMoreFiles: filesStore.loadMoreFiles,
  };
})(observer(FileList));
