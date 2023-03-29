import React, { useCallback, useContext, useEffect, useRef, useState } from 'react';
import {
    AzureMap,
    IAzureMapOptions,
    AzureMapDataSourceProvider,
    AzureMapFeature,
    IAzureMapsContextProps,
    AzureMapsContext,
} from 'react-azure-maps';
import { AuthenticationType, data } from 'azure-maps-control';
import { useParams } from 'react-router';
import { AppContext } from '../AppContext';
import { Point } from '../api';


const option: IAzureMapOptions = {
    authOptions: {
        authType: AuthenticationType.subscriptionKey,
        subscriptionKey: process.env.REACT_APP_MAPS_KEY ?? '' // Your subscription key
    },
}



const Map: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const { state } = useContext(AppContext);
    const { mapRef, isMapReady } = useContext<IAzureMapsContextProps>(AzureMapsContext);

    const [mapOptions, setMapOptions] = useState<IAzureMapOptions>(option);

    const setMapCenterAndZoom = useCallback(
        (point: data.Position, zoom: number) => {
            // setMapOptions((prevOptions) => ({
            //     ...prevOptions,
            //     center: point,
            //     zoom,
            // }));
            if (mapRef) {
                // Simple Camera options modification
                mapRef.setCamera({ center: point, zoom });
            }
        },
        [mapRef, isMapReady],
    )

    useEffect(() => {
        const point = state.points.find(p => p.id === id);
        if (point) {
            setMapCenterAndZoom(new data.Position(point.longitude!, point.latitude!), 13);
        };
    }, [id, state.points, setMapCenterAndZoom])

    return (
        <AzureMap options={mapOptions}>
            <AzureMapDataSourceProvider id="pointsDataSource"
                events={{
                    dataadded: (e: any) => {
                        console.log('Data on source added', e);
                    },
                }}>
                {state.points.map((point) => (
                    <AzureMapFeature
                        key={point.id}
                        id={point.id}
                        type="Point"
                        coordinate={new data.Position(point.longitude!, point.latitude!)}
                        properties={{
                            title: point.title,
                        }}
                    />
                ))}
            </AzureMapDataSourceProvider>
        </AzureMap>
    );
};


export default Map;