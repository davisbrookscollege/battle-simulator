import { useEffect, useState } from 'react';
import TextField from './TextField'
import IntField from './IntField';
import { createTransformer } from '../services/api';
import { getFactionNames } from '../services/api';
import RadioList from './RadioList';

export default function TransformersCreate({ data }) {

    const [name, setName] = useState('Grimlock');
    const [altVehicle, setAltVehicle] = useState('T-Rex');
    const [strength, setStrength] = useState(10);
    const [intelligence, setIntelligence] = useState(7);
    const [selectedFaction, setSelectedFaction] = useState(1);
    const [factionNames, setFactionNames] = useState([]);

    useEffect(() => {
        getFactionNames().then(({ factionList }) => {
            const filteredFactionList = factionList.filter(f => f.name !== 'Removed');
            setFactionNames(filteredFactionList);
        });
    }, []);

    return (

        <div className='transformers-create'>
            <TextField label='Name:' id='name' value={name} onChange={setName} />
            <RadioList
                items={
                    factionNames
                }
                group='faction'
                label='Faction:'
                selected={selectedFaction}
                onChange={setSelectedFaction}
            />
            <TextField label='Alternate Vehicle:' id='altVehicle' value={altVehicle} onChange={setAltVehicle} />
            <IntField label='Strength:' id='strength' value={strength} onChange={setStrength} />
            <IntField label='Intelligence:' id='intelligence' value={intelligence} onChange={setIntelligence} />
            
            <button onClick={async () => {
                
                await createTransformer({
                    name,
                    faction: selectedFaction,
                    altVehicle,
                    strength,
                    intelligence
                });

                data.onDone();
            }}>
                Create Transformer
            </button>
        </div>
    )
}