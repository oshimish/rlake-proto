import React, { createContext, useState } from 'react';
import { Conversation, Point } from './api';

interface AppState {
  error?: unknown;
  points: Point[];
  conversations: Conversation[];
}
 
interface AppContextProps extends WithChildren {
  state: AppState;
  updateState: (newState: Partial<AppState>) => void;
}

export const AppContext = createContext<AppContextProps>({
  state: {
    error: undefined,
    points: [],
    conversations: []
  },
  updateState: () => {},
});

export const AppContextConsumer = AppContext.Consumer;

export const AppContextProvider: React.FC<WithChildren> = ({ children }) => {
  const [state, setState] = useState<AppState>({
    points: [],
    conversations: [],
  });

  const updateState = (newState: Partial<AppState>) => {
    setState((prevState) => ({ ...prevState, ...newState }));
  };

  return <AppContext.Provider value={{ state, updateState }}>{children}</AppContext.Provider>
};

export default AppContextProvider;
