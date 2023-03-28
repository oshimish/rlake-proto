import React, { createContext, useState } from 'react';
import { Conversation, SearchResultDto } from './api';

interface AppState {
  locations: Location[];
  conversations: Conversation[];
  searchResult: SearchResultDto[];
  //mapData: MapData;
}
 
interface AppContextProps extends WithChildren {
  state: AppState;
  setState: React.Dispatch<React.SetStateAction<AppState>>;
}

export const AppContext = createContext<AppContextProps>({
  state: {
    locations: [],
    conversations: [],
    searchResult: [],
    //mapData: {},
  },
  setState: () => {},
});

export const AppContextConsumer = AppContext.Consumer;

export const AppContextProvider: React.FC<WithChildren> = ({ children }) => {
  const [state, setState] = useState<AppState>({
    locations: [],
    conversations: [],
    searchResult: [],
    //mapData: {},
  });

  return <AppContext.Provider value={{ state, setState }}>{children}</AppContext.Provider>
};

export default AppContextProvider;
