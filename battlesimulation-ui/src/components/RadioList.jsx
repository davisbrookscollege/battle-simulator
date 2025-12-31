import RadioButton from './RadioButton';

export default function RadioList({items, group, label, selected, onChange}) { 

    return (
        <div className="radio-list">
            {label && (
                <div>
                <label>{label}</label>
                <br />
                </div>
            )}
            {items.map(item => (
                <RadioButton
                    key={item.id}
                    item={item}
                    group={group}
                    selected={selected}
                    onChange={onChange}
                />
            ))}
        </div>
    )
}