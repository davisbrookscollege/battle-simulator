const API_BASE = '/api/transformers';

export async function fetchTransformer(id) {

    const response = await fetch(`${API_BASE}/${id}`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
    return response.json();
}

export async function fetchByFaction(faction) {

    const response = await fetch(`${API_BASE}/faction/${faction}`);
    if (!response.ok) {
        throw new Error('Network response was not ok');
    }
    return response.json();
}

export async function runBattle(battleData) {

    const response = await fetch(`${API_BASE}/battle`, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(battleData)
    });

    if (!response.ok) {
        throw new Error('Failed to post battle data');
    }

    return response.json();
}

export async function deleteTransformer(id) {

    const response = await fetch(`${API_BASE}/${id}`, {
        method: 'DELETE',
    });

    if (!response.ok) {
        throw new Error('Failed to delete transformer');
    }

    return response.json();
}

export async function createTransformer(transformerData) {

  const response = await fetch(`${API_BASE}/create`, {
    method: 'POST',
    headers: {'Content-Type': 'application/json'},
    body: JSON.stringify(transformerData)
  });
  if (!response.ok) {
    throw new Error('Failed to create transformer');
  }
  return response.json();
}

export async function getFactionNames() {

    const response = await fetch(`${API_BASE}/faction/names`);
    if (!response.ok) {
        throw new Error('Network response was not ok');
    }

    const factionList = await response.json();

    const factionMap = {};
    factionList.forEach(f => {
        factionMap[f.id] = f.name;
    });

    return { factionList, factionMap };
}