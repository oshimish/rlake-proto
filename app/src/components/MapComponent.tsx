import React from 'react';
import {AzureMap, IAzureMapOptions} from 'react-azure-maps'
import {AuthenticationType} from 'azure-maps-control'
import { useParams } from 'react-router';

const option: IAzureMapOptions = {
    authOptions: {
        authType: AuthenticationType.subscriptionKey,
        subscriptionKey: process.env.REACT_APP_MAPS_KEY ?? '' // Your subscription key
    },
}


function Map() {
    let { id } = useParams();
    console.log(id);
    return (
        <AzureMap options={option} />
    )
}


export default Map;