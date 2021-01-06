import { StatusBar } from 'expo-status-bar';
import React, {useEffect, useState } from 'react';
import { ApolloClient, InMemoryCache, ApolloProvider, createHttpLink } from '@apollo/client';
import { setContext } from '@apollo/client/link/context';
import { SafeAreaProvider } from 'react-native-safe-area-context';
import { ActivityIndicator, AsyncStorage, Button, StyleSheet, Text, View } from 'react-native';

import useCachedResources from './hooks/useCachedResources';
import useColorScheme from './hooks/useColorScheme';
import Navigation from './navigation';
import LoginScreen from './screens/LoginScreen';
import { ThemeProvider } from 'react-native-elements';
import * as AppAuth from 'expo-app-auth';

const httpLink = createHttpLink({
  uri: 'http:///10.0.0.25:32769/graphql', // TODO: http://localhost:32779/graphql
});

const authLink = setContext(async (_, { headers }) => {
  // get the authentication token from local storage if it exists
  // const authState = await getCachedAuthAsync();
  // const token = authState.idToken;
  // return the headers to the context so httpLink can read them
  return {
    headers: {
      ...headers,
      authorization: "", // token ? `Bearer ${token}` : "",
    }
  }
});

// Initialize Apollo Client
const client = new ApolloClient({
  link: authLink.concat(httpLink),
  cache: new InMemoryCache()
});

export default function App() {
  const isLoadingComplete = useCachedResources();
  const colorScheme = useColorScheme();
  
  let [authState, setAuthState] = useState(null);

  useEffect(() => {
    (async () => {
      let cachedAuth = await getCachedAuthAsync();
      if (cachedAuth && !authState) {
        setAuthState(cachedAuth);
      }
    })();
  }, []);

  const signIn = async () => {
    const _authState = await signInAsync();
    setAuthState(_authState);
  };

  const signOut = async () => {
    await signOutAsync(authState);
    setAuthState(null);
  };

  if (!isLoadingComplete) {
    return <ActivityIndicator />;
  // } else if (authState === null) {
  //   return <LoginScreen onPress={signIn}/>;
  } else {
    return (
      <ApolloProvider client={client}>
        <ThemeProvider useDark={colorScheme === 'dark'}>
          <SafeAreaProvider>
            <Navigation colorScheme={colorScheme} />
            <StatusBar />
            {/* <Text>Expo AppAuth Example: {JSON.stringify(authState, null, 2)}</Text> */}
          </SafeAreaProvider>
        </ThemeProvider>
      </ApolloProvider>
    );
  }
}

let config = {
  issuer: 'https://accounts.google.com',
  scopes: ['openid', 'profile'],
  clientId: '698621857290-hpij7m84409oqh4pgi865h2nnej2nu2m.apps.googleusercontent.com',
};

let StorageKey = '@MyApp:CustomGoogleOAuthKey';

export async function signInAsync() {
  let authState = await AppAuth.authAsync(config);
  await cacheAuthAsync(authState);
  console.log('signInAsync', authState);
  return authState;
}

async function cacheAuthAsync(authState) {
  return await AsyncStorage.setItem(StorageKey, JSON.stringify(authState));
}

export async function getCachedAuthAsync() {
  let value = await AsyncStorage.getItem(StorageKey);
  let authState = JSON.parse(value);
  console.log('getCachedAuthAsync', authState);
  if (authState) {
    if (checkIfTokenExpired(authState)) {
      return refreshAuthAsync(authState);
    } else {
      return authState;
    }
  }
  return null;
}

function checkIfTokenExpired({ accessTokenExpirationDate }) {
  return new Date(accessTokenExpirationDate) < new Date();
}

async function refreshAuthAsync({ refreshToken }) {
  let authState = await AppAuth.refreshAsync(config, refreshToken);
  console.log('refreshAuth', authState);
  await cacheAuthAsync(authState);
  return authState;
}

export async function signOutAsync({ accessToken }) {
  try {
    await AppAuth.revokeAsync(config, {
      token: accessToken,
      isClientIdProvided: true,
    });
    await AsyncStorage.removeItem(StorageKey);
    return null;
  } catch (e) {
    alert(`Failed to revoke token: ${e.message}`);
  }
}