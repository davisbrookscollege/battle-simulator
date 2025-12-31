export default function CheckBox({item, checked, onChange}) {
    return (
        <label className='checkbox'>
            <input 
                type='checkbox'
                checked={checked}
                onChange={onChange}
            />
            {item.name}
        </label>
    )
}
