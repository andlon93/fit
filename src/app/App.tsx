import { StatusBar } from 'expo-status-bar';
import React from 'react';
import { ApolloClient, InMemoryCache, ApolloProvider } from '@apollo/client';
import { SafeAreaProvider } from 'react-native-safe-area-context';
import { AppLoading } from 'expo';

import useCachedResources from './hooks/useCachedResources';
import useColorScheme from './hooks/useColorScheme';
import Navigation from './navigation';
import { ThemeProvider } from 'react-native-elements';

// Initialize Apollo Client
const client = new ApolloClient({
  uri: 'http:///10.0.0.25:32769/graphql', // TODO: http://localhost:32779/graphql
  cache: new InMemoryCache()
});

export default function App() {
  const isLoadingComplete = useCachedResources();
  const colorScheme = useColorScheme();

  if (!isLoadingComplete) {
    return <AppLoading />;
  } else {
    return (
      <ApolloProvider client={client}>
        <ThemeProvider useDark={colorScheme === 'dark'}>
          <SafeAreaProvider>
            <Navigation colorScheme={colorScheme} />
            <StatusBar />
          </SafeAreaProvider>
        </ThemeProvider>
      </ApolloProvider>
    );
  }
}
