import React, { useState, useEffect } from 'react';
import { Matricula, Aluno, Disciplina } from '../types';
import MatriculaForm from '../components/forms/MatriculaForm';
import MatriculaTable from '../components/tables/MatriculaTable';


const API_URL_MATRICULAS = 'http://localhost:8080/api/matriculas';
const API_URL_ALUNOS = 'http://localhost:8080/api/alunos';
const API_URL_DISCIPLINAS = 'http://localhost:8080/api/disciplinas';

function MatriculaPage() {
  const [matriculas, setMatriculas] = useState<Matricula[]>([]);
  const [alunos, setAlunos] = useState<Aluno[]>([]);
  const [disciplinas, setDisciplinas] = useState<Disciplina[]>([]);
  const [editingMatricula, setEditingMatricula] = useState<Matricula | undefined>(undefined);
  const [error, setError] = useState<string | null>(null);

  const fetchMatriculas = async () => {
    try {
      const response = await fetch(API_URL_MATRICULAS);
      if (!response.ok) throw new Error('Erro ao buscar matrículas.');
      const data = await response.json();
      setMatriculas(data);
    } catch (err) {
      setError('Falha ao carregar matrículas. Verifique a conexão com a API.');
      console.error(err);
    }
  };

  const fetchAlunos = async () => {
    try {
      const response = await fetch(API_URL_ALUNOS);
      if (!response.ok) throw new Error('Erro ao buscar alunos.');
      const data = await response.json();
      setAlunos(data);
    } catch (err) {
      setError('Falha ao carregar alunos para o formulário.');
      console.error(err);
    }
  };

  const fetchDisciplinas = async () => {
    try {
      const response = await fetch(API_URL_DISCIPLINAS);
      if (!response.ok) throw new Error('Erro ao buscar disciplinas.');
      const data = await response.json();
      setDisciplinas(data);
    } catch (err) {
      setError('Falha ao carregar disciplinas para o formulário.');
      console.error(err);
    }
  };

  const createOrUpdateMatricula = async (matricula: Matricula) => {
    try {
      const isUpdate = matricula.id;
      const url = isUpdate ? `${API_URL_MATRICULAS}/${matricula.id}` : API_URL_MATRICULAS;
      const method = isUpdate ? 'PUT' : 'POST';

      const matriculaToSave = {
        ...matricula,
        alunoId: Number(matricula.alunoId),
        disciplinaId: Number(matricula.disciplinaId),
        notaFinal: matricula.notaFinal ? Number(matricula.notaFinal) : undefined,
      };

      const response = await fetch(url, {
        method: method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(matriculaToSave),
      });

      if (!response.ok) {
        throw new Error(`Erro ao salvar matrícula: ${response.statusText}`);
      }

      await fetchMatriculas();
      setEditingMatricula(undefined);
    } catch (err) {
      setError('Erro ao salvar a matrícula.');
      console.error(err);
    }
  };

  const deleteMatricula = async (id: number) => {
    if (window.confirm("Tem certeza que deseja excluir esta matrícula?")) {
      try {
        const response = await fetch(`${API_URL_MATRICULAS}/${id}`, { method: 'DELETE' });
        if (!response.ok) throw new Error('Erro ao excluir matrícula.');
        await fetchMatriculas();
      } catch (err) {
        setError('Erro ao excluir a matrícula.');
        console.error(err);
      }
    }
  };

  const editMatricula = (matricula: Matricula) => {
    setEditingMatricula(matricula);
  };

  useEffect(() => {
    fetchMatriculas();
    fetchAlunos();
    fetchDisciplinas();
  }, []);

  if (error) return <div><p className="error-message">{error}</p></div>;

  return (
    <div className="container">
      <h1>Gerenciamento de Matrículas</h1>
      <div className="form-section">
        <h2>{editingMatricula ? 'Editar Matrícula' : 'Adicionar Nova Matrícula'}</h2>
        <MatriculaForm initialData={editingMatricula} onSubmit={createOrUpdateMatricula} alunos={alunos} disciplinas={disciplinas} />
        {editingMatricula && <button onClick={() => setEditingMatricula(undefined)}>Cancelar Edição</button>}
      </div>
      <div className="table-section">
        <h2>Lista de Matrículas</h2>
        <MatriculaTable data={matriculas} onEdit={editMatricula} onDelete={deleteMatricula} />
      </div>
    </div>
  );
}

export default MatriculaPage;