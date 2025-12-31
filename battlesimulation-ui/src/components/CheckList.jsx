import CheckBox from './CheckBox';

export default function CheckList({ items, selected, onToggle }) {
    return (
        <div className="check-list">
            {items.map(item => (
                <CheckBox
                    key={item.id}
                    item={item}
                    checked={selected.includes(item.id)}
                    onChange={() => onToggle(item.id)}
                />
            ))}
        </div>
    )
};