import React, { useCallback } from "react";
import AutoSizer from "react-virtualized-auto-sizer";
import { FixedSizeList as List } from "react-window";
import InfiniteLoader from "react-window-infinite-loader";
import memoize from "memoize-one";
import { inject, observer } from "mobx-react";
import { WindowScroller } from "react-virtualized";

import RowWrapper from "./RowWrapper";

const FileList = ({ items, filter, loadMoreFiles }) => {
  const createItemData = memoize((items) => ({
    items,
  }));

  const itemData = createItemData(items);

  const folderId = filter.folder;

  const isItemLoaded = (index) => !!items[index];

  const loadMoreItems = useCallback(() => {
    loadMoreFiles(folderId, filter);
  });

  return (
    <WindowScroller>
      {({ height, isScrolling, registerChild, scrollTop }) => (
        <AutoSizer disableHeight>
          {({ width, style }) => (
            <InfiniteLoader
              isItemLoaded={isItemLoaded}
              itemCount={filter.total}
              loadMoreItems={loadMoreItems}
            >
              {({ onItemsRendered, ref }) => (
                <div ref={registerChild}>
                  <List
                    style={style}
                    height={height}
                    width={width}
                    itemData={itemData}
                    itemCount={items.length}
                    itemSize={48}
                    onItemsRendered={onItemsRendered}
                    ref={ref}
                    scrollTop={scrollTop}
                    isScrolling={isScrolling}
                  >
                    {isItemLoaded ? RowWrapper : "Loading..."}
                  </List>
                </div>
              )}
            </InfiniteLoader>
          )}
        </AutoSizer>
      )}
    </WindowScroller>
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
