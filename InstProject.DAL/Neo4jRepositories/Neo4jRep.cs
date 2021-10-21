using InstProject.DAL.Models;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstProject.DAL.Neo4jRepositories
{
    public class Neo4jRep
    {
        private readonly IGraphClient _graphClient;

        public Neo4jRep()
        {
            _graphClient = new GraphClient(new Uri("http://localhost:7474/db/data"), "neouser", "neo");
            _graphClient.Connect();
        }
        public void CreatePerson(Person person)
        {
            _graphClient.Cypher
                .Create("(np:Person {newUser})")
                .WithParam("newUser", person)
                .ExecuteWithoutResults();
        }
        public void DeletePerson(Person person)
        {

            _graphClient.Cypher
                .Match("(nm:Person {nickname: {nick}})")
                .WithParam("nick", person.NickName)
                .Delete("nm")
                .ExecuteWithoutResults();

        }
        public void CreatRelationShip(Person p1, Person p2)
        {
            _graphClient.Cypher
               .Match("(p1:Person {nickname: {Nick1}})", "(p2:Person {nickname: {Nick2}})")
               .WithParam("Nick1", p1.NickName)
               .WithParam("Nick2", p2.NickName)
               .Create("(p1)-[:FOLLOW]->(p2)")
                .ExecuteWithoutResults();
        }
        public void DeleteRelationShip(Person p1, Person p2)
        {
            _graphClient.Cypher
                .Match("(p1:Person {nickname: {Nick1}})-[r:FOLLOW]->(p2:Person {nickname: {Nick2}})")
                .WithParam("Nick1", p1.NickName)
                .WithParam("Nick2", p2.NickName)
              .Delete("r")
              .ExecuteWithoutResults();
        }
        public void DeleteAllRelationShips(Person p)
        {
            _graphClient.Cypher
              .Match("(p1:Person)-[r:FOLLOW]->(p2:Person {nickname: {p1NickName}})")
              .WithParam("p1NickName", p.NickName)
              .Delete("r")
              .ExecuteWithoutResults();
            _graphClient.Cypher
             .Match("(p1:Person {nickname: {p1NickName}})-[r:FOLLOW]->(p2:Person)")
             .WithParam("p1NickName", p.NickName)
             .Delete("r")
             .ExecuteWithoutResults();
        }
        public IEnumerable<string> ShortestPath(Person person1, Person person2)
        {


            var query = _graphClient.Cypher
                  .Match("p = shortestPath((p1:Person {nickname: {Nick1}})-[*]->(p2:Person  {nickname: {Nick2}}))")
                   .WithParam("Nick1", person1.NickName)
                .WithParam("Nick2", person2.NickName)
                  .Return(() => Return.As<IEnumerable<string>>("[n IN nodes(p) | n.nickname]"));

            return query.Results.Single();
        }
    }
}
