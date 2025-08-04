import React, { useState, useEffect } from 'react';
import { Curso } from '../types';
import CursoForm from '../components/forms/CursoForm';
import CursosTable from '../components/tables/CursoTable';

const API_URL_CURSOS = 'http://localhost:8080/api/cursos';

function CursoPage() {
  const [cursos, setCursos] = useState<Curso[]>([]);
  const [editingCurso, setEditingCurso] = useState<Curso | undefined>(undefined);
  const [error, setError] = useState<string | null>(null);

  const fetchCursos = async () => {
    try {
      const response = await fetch(API_URL_CURSOS);
      if (!response.ok) throw new Error('Erro ao buscar cursos.');
      const data = await response.json();
      setCursos(data);
    } catch (err) {
      setError('Falha ao carregar cursos. Verifique a conexão com a API.');
      console.error(err);
    }
  };

  const createOrUpdateCurso = async (curso: Curso) => {
    try {
      const isUpdate = curso.id;
      const url = isUpdate ? `${API_URL_CURSOS}/${curso.id}` : API_URL_CURSOS;
      const method = isUpdate ? 'PUT' : 'POST';

      const cursoToSave = { ...curso, duracaoSemestres: Number(curso.duracaoSemestres) };

      const response = await fetch(url, {
        method: method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(cursoToSave),
      });

      if (!response.ok) throw new Error(`Erro ao salvar curso: ${response.statusText}`);

      await fetchCursos();
      setEditingCurso(undefined);
    } catch (err) {
      setError('Erro ao salvar o curso.');
      console.error(err);
    }
  };

  const deleteCurso = async (id: number) => {
    if (window.confirm("Tem certeza que deseja excluir este curso?")) {
      try {
        const response = await fetch(`${API_URL_CURSOS}/${id}`, { method: 'DELETE' });
        if (!response.ok) throw new Error('Erro ao excluir curso.');
        await fetchCursos();
      } catch (err) {
        setError('Erro ao excluir o curso.');
        console.error(err);
      }
    }
  };

  const editCurso = (curso: Curso) => {
    setEditingCurso(curso);
  };

  useEffect(() => {
    fetchCursos();
  }, []);

  if (error) return <div><p className="error-message">{error}</p></div>;

  return (
    <div className="container">
      <h1>Gerenciamento de Cursos</h1>
      <div className="form-section">
        <h2>{editingCurso ? 'Editar Curso' : 'Adicionar Novo Curso'}</h2>
        <CursoForm initialData={editingCurso} onSubmit={createOrUpdateCurso} />
        {editingCurso && <button onClick={() => setEditingCurso(undefined)}>Cancelar Edição</button>}
      </div>
      <div className="table-section">
        <h2>Lista de Cursos</h2>
        <CursosTable data={cursos} onEdit={editCurso} onDelete={deleteCurso} />
      </div>
    </div>
  );
}

export default CursoPage;