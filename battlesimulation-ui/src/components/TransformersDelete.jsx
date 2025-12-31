import { useEffect } from 'react';
import { deleteTransformer } from '../services/api';
import pluralize from '../Utilities/Pluralize';

export default function TransformersDelete({ data }) 
{

    const { ids, onDone } = data;
    const numTransformers = ids.length;

    useEffect(() => {

        async function deleteTransformers() {
            for (const id of ids) {
                try {
                    await deleteTransformer(id);
                    console.log(`Deleted transformer with ID: ${id}`);
                } catch (error) {
                    console.error(`Failed to delete transformer with ID: ${id}`, error);
                }
            }
            onDone();
        }

        deleteTransformers();
    }, [ids, onDone]);

    return (
        <div className='transformers-delete'>
            <h2>Deleted {numTransformers} {pluralize(numTransformers, 'transformer')}</h2>
        </div>
    )
}