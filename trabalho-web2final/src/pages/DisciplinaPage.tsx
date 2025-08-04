import React, { useState, useEffect } from 'react';
import { Disciplina, Curso } from '../types';
import DisciplinaForm from '../components/forms/DisciplinaForm';
import DisciplinaTable from '../components/tables/DisciplinaTable';

const API_URL_DISCIPLINAS = 'http://localhost:8080/api/disciplinas';
const API_URL_CURSOS = 'http://localhost:8080/api/cursos';

function DisciplinaPage() {
  const [disciplinas, setDisciplinas] = useState<Disciplina[]>([]);
  const [cursos, setCursos] = useState<Curso[]>([]);
  const [editingDisciplina, setEditingDisciplina] = useState<Disciplina | undefined>(undefined);
  const [error, setError] = useState<string | null>(null);

  const fetchDisciplinas = async () => {
    try {
      const response = await fetch(API_URL_DISCIPLINAS);
      if (!response.ok) throw new Error('Erro ao buscar disciplinas.');
      const data = await response.json();
      setDisciplinas(data);
    } catch (err) {
      setError('Falha ao carregar disciplinas. Verifique a conexão com a API.');
      console.error(err);
    }
  };

  const fetchCursos = async () => {
    try {
      const response = await fetch(API_URL_CURSOS);
      if (!response.ok) throw new Error('Erro ao buscar cursos.');
      const data = await response.json();
      setCursos(data);
    } catch (err) {
      setError('Falha ao carregar cursos para o formulário.');
      console.error(err);
    }
  };

  const createOrUpdateDisciplina = async (disciplina: Disciplina) => {
    try {
      const isUpdate = disciplina.id;
      const url = isUpdate ? `${API_URL_DISCIPLINAS}/${disciplina.id}` : API_URL_DISCIPLINAS;
      const method = isUpdate ? 'PUT' : 'POST';

      const disciplinaToSave = {
        ...disciplina,
        cargaHoraria: Number(disciplina.cargaHoraria),
        cursoId: Number(disciplina.cursoId),
      };

      const response = await fetch(url, {
        method: method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(disciplinaToSave),
      });

      if (!response.ok) throw new Error(`Erro ao salvar disciplina: ${response.statusText}`);

      await fetchDisciplinas();
      setEditingDisciplina(undefined);
    } catch (err) {
      setError('Erro ao salvar a disciplina.');
      console.error(err);
    }
  };

  const deleteDisciplina = async (id: number) => {
    if (window.confirm("Tem certeza que deseja excluir esta disciplina?")) {
      try {
        const response = await fetch(`${API_URL_DISCIPLINAS}/${id}`, { method: 'DELETE' });
        if (!response.ok) throw new Error('Erro ao excluir disciplina.');
        await fetchDisciplinas();
      } catch (err) {
        setError('Erro ao excluir a disciplina.');
        console.error(err);
      }
    }
  };

  const editDisciplina = (disciplina: Disciplina) => {
    setEditingDisciplina(disciplina);
  };

  useEffect(() => {
    fetchDisciplinas();
    fetchCursos();
  }, []);

  if (error) return <div><p className="error-message">{error}</p></div>;

  return (
    <div className="container">
      <h1>Gerenciamento de Disciplinas</h1>
      <div className="form-section">
        <h2>{editingDisciplina ? 'Editar Disciplina' : 'Adicionar Nova Disciplina'}</h2>
        <DisciplinaForm initialData={editingDisciplina} onSubmit={createOrUpdateDisciplina} cursos={cursos} />
        {editingDisciplina && <button onClick={() => setEditingDisciplina(undefined)}>Cancelar Edição</button>}
      </div>
      <div className="table-section">
        <h2>Lista de Disciplinas</h2>
        <DisciplinaTable data={disciplinas} onEdit={editDisciplina} onDelete={deleteDisciplina} />
      </div>
    </div>
  );
}

export default DisciplinaPage;