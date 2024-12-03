import {Injectable, Signal, signal} from '@angular/core';
import {TaskStatus} from '../models/task-status';
import { Task } from '../models/task';
import {HttpClient} from '@angular/common/http';
import {Observable, tap} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private $tasks = signal<Task[]>([]);

  constructor(private http: HttpClient) {
    this.fetchTasks();
  }

  private fetchTasks(): void {
    this.http.get<Task[]>('https://localhost:7230/tasks').subscribe((tasksFromServer) => {
      this.$tasks.set(tasksFromServer);
    });
  }

  getTasks(): Signal<Task[]> {
    return this.$tasks.asReadonly();
  }

  addTask(task: Task): Observable<number> {
    const newTask = { ...task, status: TaskStatus.NotStarted }

    return this.http.post<number>('https://localhost:7230/tasks', newTask)
      .pipe(tap(id => this.$tasks.update((tasks) => [...tasks, {
        ...newTask,
        id: id
    }])));
  }

  updateTaskStatus(id: number, newStatus: TaskStatus): Observable<void> {
    const taskToUpdate = this.$tasks().find((t) => t.id === id);
    const updatedTask = { ...taskToUpdate, status: newStatus };

    return this.http.patch<void>(`https://localhost:7230/tasks/${id}/status`, updatedTask).pipe(tap(_ => {
      this.$tasks.update((tasks) => {
        const index = tasks.findIndex((t) => t.id === id);
        const newTasks = [...tasks];
        newTasks[index] = updatedTask as Task;
        return newTasks;
      });
    }));
  }
}
