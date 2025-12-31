export default function RenderFighterList( {fighters} ) {
    return (
        <ul>
        {fighters.map(fighter => (
            <li key={fighter.id}>{fighter.name}</li>
        ))}
        </ul>
    );
}