function Home() {
    const handleResetDatabase = async () => {
        if (window.confirm("Tem certeza que deseja resetar todos os dados? Dados iniciais serão restaurados.")) {
            try {
                const response = await fetch('http://localhost:8080/api/reset', {
                    method: 'POST'
                });

                if (!response.ok) {
                    throw new Error('Erro ao resetar o banco de dados.');
                }

                alert('Banco de dados resetado com sucesso!');
                window.location.reload(); // Recarrega a página para exibir os dados iniciais
            } catch (error) {
                console.error('Falha ao resetar o banco de dados:', error);
                alert('Falha ao resetar o banco de dados.');
            }
        }
    };
    return (
        <div className="home-container">
            <h2 style={{ padding: '10px' }} > Um Sistema Acadêmico para uma gestão educacional simplificada.</h2>
            <section className="home-content">
                <div className="card">
                    <h3>Gerenciamento de Alunos</h3>
                    <p>Adicione, edite e organize os dados de todos os alunos de forma centralizada.</p>
                </div>
                <div className="card">
                    <h3>Matrículas Simplificadas</h3>
                    <p>Controle as matrículas em disciplinas, notas e histórico acadêmico com facilidade.</p>
                </div>
                <div className="card">
                    <h3>Cursos e Disciplinas</h3>
                    <p>Administre a estrutura de cursos e a ementa das disciplinas de todo o currículo.</p>
                </div>
            </section>
            <button onClick={handleResetDatabase}>
                Resetar Dados
            </button>
        </div>
    );
}

export default Home;