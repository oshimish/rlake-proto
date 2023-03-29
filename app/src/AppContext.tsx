import React, { createContext, useState } from 'react';
import { Conversation, Point, SearchResultDto } from './api';

interface AppState {
  points: Point[];
  conversations: Conversation[];
  searchResult: SearchResultDto;
  //mapData: MapData;
}
 
interface AppContextProps extends WithChildren {
  state: AppState;
  //setState: React.Dispatch<React.SetStateAction<AppState>>;
  updateState: (newState: Partial<AppState>) => void;
}

export const AppContext = createContext<AppContextProps>({
  state: {
    points: [],
    conversations: [],
    searchResult: new SearchResultDto({ items:[] }),
    //mapData: {},
  },
  //setState: () => {},
  updateState: () => {},
});

export const AppContextConsumer = AppContext.Consumer;

export const AppContextProvider: React.FC<WithChildren> = ({ children }) => {
  const [state, setState] = useState<AppState>({
    points: [],
    conversations: [],
    searchResult: new SearchResultDto({ items:[] }),
  });

  const updateState = (newState: Partial<AppState>) => {
    setState((prevState) => ({ ...prevState, ...newState }));
  };

  return <AppContext.Provider value={{ state, updateState }}>{children}</AppContext.Provider>
};

export default AppContextProvider;
