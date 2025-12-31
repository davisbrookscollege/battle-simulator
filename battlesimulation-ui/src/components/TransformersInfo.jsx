import { useEffect, useState } from 'react';
import { fetchTransformer } from '../services/api';
import IndividualTransformerInfo from './IndividualTransformerInfo';

export default function TransformersInfo({ ids }) {

    const [transformers, setTransformers] = useState([]);

    useEffect(() => {

        async function fetchTransformers() {
            const transformers = await Promise.all(
                ids.map(id => fetchTransformer(id))
            );
            setTransformers(transformers);
        }

        fetchTransformers();
    }, [ids]);

    return (
        <div className='transformers-info'>
            <h2>Transformer Information:</h2>
            
            {transformers.length === 0 ? (
                <div className='transformers-info-empty'>
                    <p>No transformers were selected</p>
                </div>
            ) : (
                transformers.map(transformer => (
                    <IndividualTransformerInfo key={transformer.id} transformer={transformer} />
                ))
            )}
        </div>
    )
}