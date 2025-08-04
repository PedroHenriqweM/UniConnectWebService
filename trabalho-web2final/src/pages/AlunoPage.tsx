import React, { useState, useEffect } from 'react';
import { Aluno, Curso } from '../types';
import AlunoForm from '../components/forms/AlunoForm';
import AlunoTable from '../components/tables/AlunoTable';


const API_URL_ALUNOS = 'http://localhost:8080/api/alunos'; 
const API_URL_CURSOS = 'http://localhost:8080/api/cursos';

function AlunoPage() {
    const [alunos, setAlunos] = useState<Aluno[]>([]);
    const [cursos, setCursos] = useState<Curso[]>([]);
    const [editingAluno, setEditingAluno] = useState<Aluno | undefined>(undefined);
    const [error, setError] = useState<string | null>(null);

    const fetchAlunos = async () => {
        try {
            const response = await fetch(API_URL_ALUNOS);
            if (!response.ok) {
                throw new Error('Erro ao buscar alunos.');
            }
            const data = await response.json();
            setAlunos(data);
        } catch (err) {
            setError('Falha ao carregar alunos. Verifique a conexão com a API.');
            console.error(err);
        }
    };

    const fetchCursos = async () => {
        try {
            const response = await fetch(API_URL_CURSOS);
            if (!response.ok) {
                throw new Error('Erro ao buscar cursos.');
            }
            const data = await response.json();
            setCursos(data);
        } catch (err) {
            setError('Falha ao carregar cursos para o formulário.');
            console.error(err);
        }
    };

    const createOrUpdateAluno = async (aluno: Aluno) => {
        try {
            const isUpdate = aluno.id;
            const url = isUpdate ? `${API_URL_ALUNOS}/${aluno.id}` : API_URL_ALUNOS;
            const method = isUpdate ? 'PUT' : 'POST';

            const alunoToSave = {
                ...aluno,
                dataNascimento: new Date(aluno.dataNascimento).toISOString(),
                cursoId: Number(aluno.cursoId),
            };

            const response = await fetch(url, {
                method: method,
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(alunoToSave),
            });

            if (!response.ok) {
                throw new Error(`Erro ao salvar aluno: ${response.statusText}`);
            }

            await fetchAlunos();
            setEditingAluno(undefined);
        } catch (err) {
            setError('Erro ao salvar o aluno.');
            console.error(err);
        }
    };

    const deleteAluno = async (id: number) => {
        if (window.confirm("Tem certeza que deseja excluir este aluno?")) {
            try {
                const response = await fetch(`${API_URL_ALUNOS}/${id}`, {
                    method: 'DELETE',
                });

                if (!response.ok) {
                    throw new Error('Erro ao excluir aluno.');
                }
                await fetchAlunos();
            } catch (err) {
                setError('Erro ao excluir o aluno.');
                console.error(err);
            }
        }
    };

    const editAluno = (aluno: Aluno) => {
        setEditingAluno(aluno);
    };

    useEffect(() => {
        fetchAlunos();
        fetchCursos();
    }, []);

    if (error) return <div><p className="error-message">{error}</p></div>;

    return (
        <div className="container">
            <h1>Gerenciamento de Alunos</h1>

            <div className="form-section">
                <h2>{editingAluno ? 'Editar Aluno' : 'Adicionar Novo Aluno'}</h2>
                <AlunoForm
                    initialData={editingAluno}
                    onSubmit={createOrUpdateAluno}
                    cursos={cursos}
                />
                {editingAluno && <button onClick={() => setEditingAluno(undefined)}>Cancelar Edição</button>}
            </div>

            <div className="table-section">
                <h2>Lista de Alunos</h2>
                <AlunoTable
                    data={alunos}
                    onEdit={editAluno}
                    onDelete={deleteAluno}
                />
            </div>
        </div>
    );
}

export default AlunoPage;